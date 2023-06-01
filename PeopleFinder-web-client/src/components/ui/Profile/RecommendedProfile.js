import React, { useState } from 'react'
import classes from './RecommendedProfile.module.css'
import profileclasses from '../Profile.module.css'
import { Link } from 'react-router-dom'
import useProfileApiActions from '../../../hooks/useProfileApiActions'


function RecommendedProfile({ profile }) {

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
        {
          profile.mutualFriends?.length > 0 &&
          <div className={classes.mutualfriends}>
            <span className='marginright10'>Mutual friends: </span>
            {
              profile.mutualFriends?.map((friend, index) => (
                <Link className='nondecoration' to={`profile/${friend}`} key={index}>
                  @{friend}{index < profile.mutualFriends.length - 1 ? "," : ""}&nbsp;
                </Link>
              ))
            }
          </div>
        }
        <Link to={`profile/${profile.username}`} className='username nondecoration'>@{profile.username}</Link>
        <div className='flexgrow'>
          <span className={classes.bio}>{profile.bio}</span>
        </div>
        {
          profile.tags?.length > 0 &&
          <ul className={profileclasses.interests}>
            {profile.tags?.map((interest) => (
              <li key={interest.id}>{interest.title}</li>
            ))}
          </ul>
        }
        {
          status === 'none' ?
            <button className={`${profileclasses.approve} justifyselfend`} onClick={handleAddToFriends}>Add friend</button>
            :
            <button className={`${profileclasses.decline} justifyselfend`} onClick={handleCancelFriendRequest}>Cancel request</button>
        }
      </div>
    )
  }

  export default RecommendedProfile