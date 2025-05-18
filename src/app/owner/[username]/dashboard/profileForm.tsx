'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { Paragraph } from "@/components/ui/Paragraph";
import { Button } from "@/components/ui/Button";
import Image from "next/image";
import { Controller, useForm, useWatch } from "react-hook-form";
import { ProfileFormData, profileSchema } from "@/lib/schemas/profileSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormInput } from "@/components/ui/FormInput";
import { List } from "@/components/ui/List";
import { FormCheckbox } from "@/components/ui/FormCheckbox";
import { addEditEducation } from "@/lib/apis/owner/addEditEducation";
import { educationListQuery } from "@/lib/apis/owner/educationListQuery";
import { useEffect, useMemo } from "react";
import { institutionListQuery } from "@/lib/apis/owner/education/institutionListQuery";
import { FormDropdown } from "@/components/ui/FormDropdown";
import { getSelectedOption, mapEducationToForm } from "@/lib/utils/appFunctions";
import { CUDModal } from "@/components/ui/CUDModal";
import { editProfile } from "@/lib/apis/owner/user/editProfile";
import { userInfoQuery } from "@/lib/apis/owner/user/userInfoQuery";

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
        formState: { errors },
    } = useForm<ProfileFormData>({
        resolver: zodResolver(profileSchema)
    });

    useEffect(() => {
        if (user) {
            const typedUser = user as ProfileFormData;
            reset({
                ...typedUser,
                birthDate: typedUser.birthDate?.slice(0, 10)
            });
        }
    }, [user]);

    const onSubmit = async (data: ProfileFormData) => {
        await dispatch(editProfile(data));
        await dispatch(userInfoQuery());
        onClose?.();
    };

    const ControlledDropdown = ({
        name,
        label,
        options,
    }: {
        name: keyof ProfileFormData;
        label: string;
        options: { label: string; value: string }[];
    }) => (
        <Controller
            name={name}
            control={control}
            render={({ field }) => (
            <FormDropdown
                label={label}
                options={options}
                value={getSelectedOption(options, field.value as string)}
                onChange={(option) => field.onChange(option?.value ?? '')}
                onBlur={field.onBlur}
                error={errors[name]}
                isLoading={options.length === 0}
            />
            )}
        />
    );

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
                    type="text"
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

                <ControlledDropdown name="gender" label="Gender" options={genderOptions} />

                <CUDModal as='update' title='Update profile picture' subTitle='Choose a new profile picture'>

                </CUDModal>

                <CUDModal as='update' title='Update cover photo' subTitle='Choose a new cover photo'>

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