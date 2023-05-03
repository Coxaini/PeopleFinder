import React, { useRef } from 'react'
import '../css/editform.css'

import { useEffect, useState } from 'react';
import useUserData from '../hooks/useUserData';
import useApiPrivate from '../hooks/useApiPrivate';
import LoaderSpinner from '../components/ui/LoaderSpinner';

function ProfileEdit() {

    const { userData } = useUserData();
    const apiPrivate = useApiPrivate();
    const [profile, setProfile] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    const fileUpload = useRef(null);
    const [imagePreviewUrl, setImagePreviewUrl] = useState('https://via.placeholder.com/150');
   

    //generate states for each input
    const [name, setName] = useState('');
    const [bio, setBio] = useState('');
    const [city, setCity] = useState('');

    const [imageErrorMsg, setImageErrorMsg] = useState('');
    const [imageLoading, setImageLoading] = useState(false);

    useEffect(() => {
        apiPrivate.get(`/profile/${userData.userid}`)
            .then(response => {
                setProfile(response?.data);
            })
            .catch(error => {
                console.log(error);
            }).finally(() => {
                setIsLoading(false);
            });


    }, [userData.userid, apiPrivate]);

    useEffect(() => {
        if (profile) {
            setName(profile.name);
            setBio(profile.bio);
            setCity(profile.city);
        }
    }, [profile]);

    if (isLoading) {
        return <div>Loading...</div>;
    }

    function handleImageChange(e) {
        e.preventDefault();  
        setImageErrorMsg("");
        if (!e.target.files || !e.target.files[0]){
            return;
        }
           
        let reader = new FileReader();
        let file = e.target.files[0];
        if(file.size > 5_000_000){
            setImageErrorMsg("File size is too big. Max size is 5MB");
            return;
        }
        
        setImageLoading(true);
        handleImageSubmit(file);
        reader.onloadend = () => {
            setImagePreviewUrl(reader.result);
        }

        reader.readAsDataURL(file)
    }

    function handleImageSubmit(imagefile) {
        let formData = new FormData();
        formData.append('imageFile', imagefile);

        apiPrivate.put('/profile/picture', formData)
            .then(response => {
                setImageLoading(false);
               
            })
            .catch(error => {
                console.log(error);

                setImageErrorMsg("An error occurred while uploading the image");
                setImageLoading(false);
                setImagePreviewUrl('https://via.placeholder.com/150');
            });
    }

    return (
        <div className='centerpanel'>
            <div className='editform'>
                <div className='flexlist'>

                    <div className='input-group'>
                        <div className='avatar-image-container'> 
                            <img src={imagePreviewUrl} onClick={() => fileUpload.current.click()} className='avatar-image' alt='profile' />
                            <div className= {`spinner-container ${imageLoading? `` : `nonvisible`}`}>
                                <LoaderSpinner scale={1.2} />
                            </div>
                        </div>
                        <div className='avatar-actions'>
                            <span className='username marginbottom10'>@{profile.username}</span>
                            <button className='upload-btn' onClick={() => fileUpload.current.click()}>Upload new photo</button>
                            <form method="post" encType="multipart/form-data" >
                                <input type='file' id='upload' accept="image/jpeg,image/png"
                                    onChange={(e) => handleImageChange(e)} ref={fileUpload} hidden />
                            </form>
                        </div>
                        
                    </div>
                    <span className={`marginbottom10 errormessage ${imageErrorMsg ===''? 'nonvisible' : '' }`}>{imageErrorMsg}</span>
                    <form>
                        <div className='input-group'>
                            <div className='label-container'>
                                <label htmlFor="name">Name:</label>
                            </div>
                            <input type="text" id="name" name="name" value={name}
                                onChange={(e) => setName(e.target.value)} required />
                        </div>
                        <div className='input-group'>
                            <div className='label-container'>
                                <label htmlFor="bio">Bio:</label>
                            </div>
                            <textarea id="bio" name="bio" value={bio} onChange={(e) => setBio(e.target.value)} />
                        </div>
                        <div className='input-group'>
                            <div className='label-container'>
                                <label htmlFor="city" >City:</label>
                            </div>
                            <input type="text" id="city" name="city" value={city} onChange={(e) => setCity(e.target.value)} />
                        </div>
                        <div id="image-preview"></div>
                        <div className='input-group'>
                            <div className='label-container'>
                                <label htmlFor="interests">Interests:</label>
                            </div>
                            <select id="interests" name="interests[]" multiple>
                                <option value="music">Music</option>
                                <option value="movies">Movies</option>
                                <option value="books">Books</option>
                                <option value="sports">Sports</option>
                                <option value="travel">Travel</option>
                            </select>
                        </div>
                        <button type="submit">Save</button>
                    </form>

                </div>
            </div >
        </div >
    )
}

export default ProfileEdit