
import { Link, redirect, useParams } from 'react-router-dom';
import useApiPrivate from '../hooks/useApiPrivate';
import { useEffect, useState } from 'react';
import useUserData from '../hooks/useUserData';

import classes from './ProfileCard.module.css'

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faLocationDot } from '@fortawesome/free-solid-svg-icons';
import { useNavigate } from 'react-router-dom';
import OverlayCentredPanel from '../components/ui/Overlay/OverlayCentredPanel';
import MutualFriendsList from '../components/ui/Profile/MutualFriendsList';
import ProfileActions from '../components/ui/Profile/ProfileActions';
import { useTranslation } from 'react-i18next';

function ProfilePage() {

    const {t} = useTranslation();
    const params = useParams();
    const [userData] = useUserData();

    const apiPrivate = useApiPrivate();
    const [profile, setProfile] = useState({});
    const [mutualFriendsCount, setMutualFriendsCount] = useState(0);
    const [isLoading, setIsLoading] = useState(true);
    const [isError, setIsError] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');

    const [mutualFriendsOverlay, setMutualFriendsOverlay] = useState(false);

    useEffect(() => {

        setMutualFriendsOverlay(false);
        
        apiPrivate.get(`/profile/${params.username ?? params.id ?? userData.id}`)
            .then(response => {
                setProfile(response?.data);
                const metadata = JSON.parse(response?.headers?.['x-pagination']);
                setMutualFriendsCount(metadata?.TotalCount);
                setIsLoading(false);
            })
            .catch(error => {

                console.log(error);
            });
       

    }, [params.username, params.id, userData.id, apiPrivate]);


    if (isLoading) {
        return <div>{t("common.loading")}</div>;
    }

    let mutualFriends = null;

    if (profile.mutualFriends.length > 0) {
        mutualFriends = (
            <Link onClick={toggleMutualFriendsOverlay} className={`${classes.horizontallayout} ${classes.textlink}`}>
                <span className={classes.rightoffset}>{t("profile.friendWith")}</span>
                {profile.mutualFriends.map((friend, i) => (
                    <div key={i}>
                        {i > 0 ? <span>, </span> : null}
                        <span className={classes.highlight}>{friend}</span>
                    </div>
                ))}
                {mutualFriendsCount > profile.mutualFriends.length ?
                    <span className={classes.leftoffset}>
                        {t("profile.andMore", mutualFriendsCount - profile.mutualFriends.length)}
                    </span> : null}

            </Link>
        );
    }

    function toggleMutualFriendsOverlay() {
        setMutualFriendsOverlay((prev) => !prev);
    }

    return (
        <>
            {mutualFriendsOverlay ?
                <OverlayCentredPanel onClick={toggleMutualFriendsOverlay} title={t("profile.mutualFriends")}>
                    <MutualFriendsList profileId={profile.id} />
                </OverlayCentredPanel> : null}

            <div className='centerpanel'>
                <div className={classes.profile}>
                    <div className={classes.profilegrid}>
                        <div className='flexlist'>
                            <img src={profile.mainPictureUrl} alt='profile' />
                            <div className={classes.actionslayout}>
                            <ProfileActions id={profile.id} status={profile.status} 
                            setStatus={(status)=>{
                                return setProfile((prev)=>{
                                    return {...prev, status}
                                 });
                            } } />
                        </div>
                        </div>
                        <div className={classes.profilepanel}>
                            <div className={classes.info}>
                                <h1>{profile.name}{profile.age ? `, ${profile.age}` : ''}</h1>
                                {mutualFriends}
                                {
                                profile.city &&
                                <div className={classes.location}>
                                    <FontAwesomeIcon icon={faLocationDot} style={{ color: "#000000", }} />
                                    <span>{profile.city}</span>
                                </div>
                                }
                                {profile.tags.length > 0 ?
                                    <div>
                                        <ul className={classes.interests}>
                                            {profile.tags?.map((interest) => (
                                                <li key={interest.id}>{t(`tags.${interest.title}`)}</li>
                                            ))}
                                        </ul>
                                    </div>
                                    : null
                                }
                                {
                                profile.bio &&
                                <>
                                <h2>{t("profile.bio")}</h2>
                                <span className={classes.bio}>{profile.bio}</span>
                                </>
                                }   

                            </div>

                        </div>
                       
                    </div>

                </div>
            </ div>
        </>
    );

}

export default ProfilePage;