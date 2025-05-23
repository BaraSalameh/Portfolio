'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { Paragraph } from "@/components/ui/Paragraph";
import { Button } from "@/components/ui/form/Button";
import Image from "next/image";
import { useForm } from "react-hook-form";
import { ProfileFormData, profileSchema } from "@/lib/schemas/profileSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { List } from "@/components/ui/List";
import { useEffect } from "react";
import { CUDModal } from "@/components/ui/CUDModal";
import { editProfile } from "@/lib/apis/owner/user/editProfile";
import { userInfoQuery } from "@/lib/apis/owner/user/userInfoQuery";
import { ControlledDropdown, FormInput, ImageUploader } from "@/components/ui/form";

const ProfileForm = ({ onClose } : { onClose?: () => void }) => {

    const dispatch = useAppDispatch();
    const { loading, error, user } = useAppSelector((state) => state.owner);
    const genderOptions = [
        { label: 'Female', value: '0' },
        { label: 'Male', value: '1' }
    ];

    const {
        register,
        handleSubmit, 
        control,
        reset,
        setValue,
        formState: { errors },
    } = useForm<ProfileFormData>({
        resolver: zodResolver(profileSchema)
    });

    useEffect(() => {
        if (user) {
            
            reset({
                ...user,
                birthDate: user.birthDate?.slice(0, 10)
            });
        }
    }, [user]);

    const onSubmit = async (data: ProfileFormData) => {
        await dispatch(editProfile(data));
        await dispatch(userInfoQuery());
        onClose?.();
    };

    const handleProfilePicture = (url: string) => {
        setValue("profilePicture", url);
    };

    const handleCoverPhoto = (url: string) => {
        setValue("coverPhoto", url);
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)} className="relative space-y-4">
            <fieldset disabled={loading} className="space-y-4">
                
                <FormInput
                    label="Firstname"
                    type="text"
                    placeholder="John"
                    registration={register('firstname')}
                    error={errors.firstname}
                />

                <FormInput
                    label="Lastname"
                    type="text"
                    placeholder="Doe"
                    registration={register('lastname')}
                    error={errors.lastname}
                />

                <FormInput
                    label="Title"
                    type="text"
                    placeholder="Software Developer"
                    registration={register('title')}
                    error={errors.title}
                />

                <FormInput
                    label="Bio"
                    type="textarea"
                    placeholder="Describe yourself"
                    registration={register('bio')}
                    error={errors.bio}
                />

                <FormInput
                    label="Phone"
                    type="text"
                    placeholder="+0 123456789"
                    registration={register('phone')}
                    error={errors.phone}
                />

                <ControlledDropdown control={control} errors={errors} name="gender" label="Gender" options={genderOptions} />

                <CUDModal as='update' title='Update profile picture' subTitle='Choose a new profile picture'>
                    <ImageUploader onAction={handleProfilePicture} preset="Profile_Picture"/>
                </CUDModal>

                <CUDModal as='update' title='Update cover photo' subTitle='Choose a new cover photo'>
                    <ImageUploader onAction={handleCoverPhoto} preset="Cover_Photo" />
                </CUDModal>

                <FormInput
                    label="Birthdate"
                    type="date"
                    registration={register('birthDate')}
                    error={errors.birthDate}
                />
            </fieldset>
            

            {Array.isArray(error) && error.length > 1 ? (
                <List intent="danger" size="sm">
                    {error.map((e: string, i: number) => (
                        <li key={i}>{e}</li>
                    ))}
                </List>
            ) : (
                error && <Paragraph intent="danger" size="sm">{error}</Paragraph>
            )}

            <Button intent="standard" rounded="full" size="lg" type="submit" disabled={loading}>
                <Image
                    className="dark:invert"
                    src="/vercel.svg"
                    alt="Vercel logomark"
                    width={20}
                    height={20}
                />
                {loading ? 'Submitting...' : 'Submit'}
            </Button>
        </form>
    );
}

export default ProfileForm;