import React, { useRef } from 'react'
import '../css/editform.css'

import { useEffect, useState } from 'react';
import useUserData from '../hooks/useUserData';
import useApiPrivate from '../hooks/useApiPrivate';
import LoaderSpinner from '../components/ui/LoaderSpinner';
import TagsSelection from '../components/ui/Profile/Tags/TagsSelection';
import { useTranslation } from 'react-i18next';

function ProfileEdit() {

    const { t } = useTranslation();

    const [userData, setUserData] = useUserData();
    const apiPrivate = useApiPrivate();
    const [profile, setProfile] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    const fileUpload = useRef(null);
    const [imagePreviewUrl, setImagePreviewUrl] = useState('');


    //generate states for each input
    const [name, setName] = useState('');
    const [bio, setBio] = useState('');
    const [city, setCity] = useState('');
    const [birthdate, setBirthdate] = useState('');
    const [gender, setGender] = useState('');
    const [selectedTags, setSelectedTags] = useState([]);
    const [username, setUsername] = useState('');

    const [imageErrorMsg, setImageErrorMsg] = useState('');
    const [imageLoading, setImageLoading] = useState(false);




    useEffect(() => {
        apiPrivate.get(`/profile/${userData.id}`)
            .then(response => {
                setProfile(response?.data);
                setImagePreviewUrl(response?.data?.mainPictureUrl);
            })
            .catch(error => {
                console.log(error);
            }).finally(() => {
                setIsLoading(false);
            });


    }, [userData.id, apiPrivate]);

    useEffect(() => {
        if (profile) {
            setName(profile.name);
            setBio(profile.bio);
            setCity(profile.city);
            setBirthdate(profile.birthDate ?? '');
            setUsername(profile.username);
            switch (profile.gender) {
                case 1: setGender('male'); break;
                case 2: setGender('woman'); break;
                default: setGender('none'); break;
            }
            setSelectedTags(profile.tags);

        }
    }, [profile]);

    if (isLoading) {
        return <div>Loading...</div>;
    }

    function handleProfileDataSubmit(e) {
        e.preventDefault();

        let numberGender = 0;
        if (gender === 'male') {
            numberGender = 1;
        } else if (gender === 'woman') {
            numberGender = 2;
        }

        apiPrivate.put(`/profile`, {
            name,
            bio,
            city,
            gender: numberGender,
            username,
            birthdate,
            tags: selectedTags.map(tag => tag.title)
        })
            .then(response => {

                setUserData({ ...userData, username: response.data?.username });
            })
            .catch(error => {
                console.log(error);
            });
    }

    function handleImageChange(e) {
        e.preventDefault();
        setImageErrorMsg("");
        if (!e.target.files || !e.target.files[0]) {
            return;
        }

        let reader = new FileReader();
        let file = e.target.files[0];
        if (file.size > 5_000_000) {
            setImageErrorMsg(t("profileEdit.imageSizeError"));
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


        apiPrivate.post('/profile/picture', formData)
            .then(response => {
                setImageLoading(false);

            })
            .catch(error => {
                console.log(error);

                setImageErrorMsg(t("profileEdit.imageUploadError"));
                setImageLoading(false);
                setImagePreviewUrl(profile?.data?.mainPictureUrl);
            });


    }

    return (
        <div className='centerpanel'>
            <div className='editform'>
                <div className='flexlist'>

                    <div className='image-input-group'>
                        <div className='avatar-image-container'>
                            <img src={imagePreviewUrl} onClick={() => fileUpload.current.click()} className='avatar-image' alt='profile' />
                            <div className={`spinner-container ${imageLoading ? `` : `nonvisible`}`}>
                                <LoaderSpinner scale={1.2} />
                            </div>
                        </div>
                        <div className='avatar-actions'>
                            <span className='username marginbottom10'>@{username}</span>
                            <button className='upload-btn' onClick={() => fileUpload.current.click()}>
                                {t("profileEdit.uploadNewPhoto")}
                            </button>
                            <form method="post" encType="multipart/form-data" >
                                <input type='file' id='upload' accept="image/jpeg,image/png"
                                    onChange={(e) => handleImageChange(e)} ref={fileUpload} hidden />
                            </form>
                        </div>

                    </div>
                    <span className={`marginbottom10 errormessage ${imageErrorMsg === '' ? 'nonvisible' : ''}`}>{imageErrorMsg}</span>
                    <form onSubmit={handleProfileDataSubmit}>
                        <label htmlFor="name">{t("profileEdit.name")}</label>
                        <input type="text" id="name" name="name" value={name}
                            onChange={(e) => setName(e.target.value)} required />

                        <label htmlFor="username">{t("registration.username")}</label>
                        <input type="text" id="username" name="username" value={username}
                            onChange={(e) => setUsername(e.target.value)} required />



                        <label htmlFor="bio">{t("profile.bio")}</label>

                        <textarea id="bio" name="bio" value={bio} onChange={(e) => setBio(e.target.value)} />

                        <label htmlFor="city" >{t("profileEdit.city")}</label>

                        <input type="text" id="city" name="city" value={city} onChange={(e) => setCity(e.target.value)} />


                        <label htmlFor='birthdate'>{t("profileEdit.birthdate")}</label>

                        <input type='date' id='birthdate' name='birthdate' value={birthdate}
                            onChange={(e) => setBirthdate(e.target.value)} />



                        <label htmlFor='gender'>{t("profileEdit.gender")}</label>

                        <select id='gender' name='gender' value={gender}
                            onChange={(e) => setGender(e.target.value)}>
                            <option value={"none"}>{t("profileEdit.none")}</option>
                            <option value={"male"}>{t("profileEdit.male")}</option>
                            <option value={"woman"}>{t("profileEdit.woman")}</option>
                        </select>



                        <label htmlFor="interests">{t("profileEdit.interests")}</label>

                        <TagsSelection selectedTags={selectedTags} setSelectedTags={setSelectedTags} />

                        <button type="submit">{t("common.save")}</button>
                    </form>

                </div>
            </div >
        </div >
    )
}

export default ProfileEdit