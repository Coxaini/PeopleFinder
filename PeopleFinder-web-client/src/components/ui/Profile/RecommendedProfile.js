import React, { useState } from 'react'
import classes from './RecommendedProfile.module.css'
import profileclasses from '../Profile.module.css'
import { Link } from 'react-router-dom'
import useProfileApiActions from '../../../hooks/useProfileApiActions'
import { useTranslation } from 'react-i18next';

function RecommendedProfile({ profile }) {

  const { t } = useTranslation();

  const { addToFriends, cancelFriendRequest } = useProfileApiActions({ id: profile.id });

  const [status, setStatus] = useState('none');

  function handleAddToFriends() {
    addToFriends().then(() => {
      setStatus('requestsent');
    }).catch((err) => {
      console.log(err);
    });
  }
  function handleCancelFriendRequest() {
    cancelFriendRequest().then(() => {
      setStatus('none');
    }).catch((err) => {
      console.log(err);
    });
  }

  return (
    <div className={classes.recprofile}>
      <div className={classes.name}>{profile.name + ", " + profile.age} </div>
      <img className={classes.image} src={profile.mainPictureUrl} alt="profile" />
      <Link to={`/profile/${profile.username}`} className='username nondecoration'>@{profile.username}</Link>
      <div className='flexgrow'>
        <span className={classes.bio}>{profile.bio}</span>
      </div>
      {
        profile.tags?.length > 0 &&
        <ul className={profileclasses.interests}>
          {profile.tags?.map((interest) => (
            <li key={interest.id}>{t(`tags.${interest.title}`)}</li>
          ))}
        </ul>
      }
      {
        profile.mutualFriends?.length > 0 &&
        <div className={classes.mutualfriends}>
          <span className='marginright10'>{t("profile.mutualFriends")}</span>
          {
            profile.mutualFriends?.map((friend, index) => (
              <Link className='nondecoration' to={`/profile/${friend}`} key={index}>
                @{friend}{index < profile.mutualFriends.length - 1 ? "," : ""}&nbsp;
              </Link>
            ))
          }
          {
            profile.mutualFriendsCount > profile.mutualFriends?.length ?
              <span>+ {profile.mutualFriendsCount - profile.mutualFriends?.length} more</span>
              : null
          }
        </div>
      }
      {
        status === 'none' ?
          <button className={`${profileclasses.approve} justifyselfend margintop10`} onClick={handleAddToFriends}>
            {t("profile.addToFriends")}
          </button>
          :
          <button className={`${profileclasses.decline} justifyselfend margintop10`} onClick={handleCancelFriendRequest}>
            {t("profile.cancelRequest")}
          </button>
      }
    </div>
  )
}

export default RecommendedProfile