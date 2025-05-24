'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { ProfileFormData, profileSchema } from "@/lib/schemas";
import { editProfile, userInfoQuery } from "@/lib/apis/owner/user";
import { ControlledForm, ImageUploader } from "@/components/ui/form";

const ProfileForm = ({ onClose } : { onClose?: () => void }) => {

    const dispatch = useAppDispatch();
    const { loading, error, user } = useAppSelector((state) => state.owner);
    const genderOptions = [
        { label: 'Female', value: '0' },
        { label: 'Male', value: '1' }
    ];

    const onSubmit = async (data: ProfileFormData) => {
        await dispatch(editProfile(data));
        await dispatch(userInfoQuery());
        onClose?.();
    };

    return (
        <ControlledForm
            schema={profileSchema}
            onSubmit={onSubmit}
            items={[
                {as: 'Input', name: 'firstname', label: 'Firstname', placeholder: 'John'},
                {as: 'Input', name: 'lastname', label: 'Lastname', placeholder: 'Doe'},
                {as: 'Input', name: 'title', label: 'Title', placeholder: 'Sr. Next.js Developer'},
                {as: 'Input', name: 'bio', label: 'Bio', placeholder: 'Describe yourself', type: 'Textarea'},
                {as: 'Input', name: 'phone', label: 'Phone', placeholder: '+0 123456789'},
                {
                    as: 'Modal',
                    name: 'profilePicture',
                    modal: {
                        as: 'update',
                        children: <ImageUploader preset="Profile_Picture"/>,
                        title:'Update profile picture',
                        subTitle: 'Choose a new profile picture'
                    }
                },
                {
                    as: 'Modal',
                    name: 'coverPhoto',
                    modal: {
                        as: 'update',
                        children: <ImageUploader preset="Cover_Photo" />,
                        title:'Update cover photo',
                        subTitle: 'Choose a new cover photo'
                    }
                },
                {as: 'Dropdown', name: 'gender', label: 'Gender', options: genderOptions},
                {as: 'Input', name: 'birthDate', label: 'Birth date', type: 'Date'}
            ]}
            error={error}
            loading={loading}
            resetItems={{
                ...user as any,
                gender: user?.gender?.toString()
            }}
            indicator={{when: 'Update', while: 'Updating...'}}
        />
    );
}

export default ProfileForm;