// components/Header.tsx
import { Header } from '@/components/shared/Header';
import Image from 'next/image';
import React from 'react';
import { Paragraph } from '../Paragraph';
import { CUDModal } from '../CUDModal';
import ProfileForm from '@/app/owner/[username]/dashboard/profileForm';

interface ProfileProps {
    user: {
        firstname: string;
        lastname: string;
        profilePicture?: string;
        title?: string;
    },
    className?: string;
}

const Profile = ({ 
    user,
    className
} : ProfileProps) => {

    const profilePicture = user?.profilePicture ?? '/globe.svg';
    const coverPhoto = user?.profilePicture ?? '/cover.jpeg';

  return (
    <Header space='lg' paddingY='sm' itemsY='start' className={`grid grid-cols-1 ${className}`}>
        <div className="relative h-35 sm:h-50 lg:h-60 ">
            {/* Cover Photo */}
            <Image
                src={coverPhoto}
                alt="Cover photo"
                fill
                className="object-fill rounded-3xl "
                priority
            />
            
            {/* Profile and Info */}
            <div className="absolute left-7 sm:left-10 lg:left-15 bottom-[-2rem] sm:bottom-[-2.5rem] lg:bottom-[-3.5rem] flex flex-col items-center">
                <div className="relative w-20 h-20 sm:w-30 sm:h-30 lg:w-40 lg:h-40 rounded-full border-4 border-white bg-black/25 backdrop-blur-sm overflow-hidden">
                    <Image
                        src={profilePicture}
                        alt="Profile picture"
                        fill
                        className="object-cover"
                    />
                </div>
                    <div className='absolute right-0 bottom-0'>
                        <CUDModal as='update' subTitle='Update profile' >
                            <ProfileForm />
                        </CUDModal>
                    </div>
            </div>
        </div>
        <div className='px-7 sm:px-10 lg:px-15 pt-1'>
            <Paragraph position='start' size='lg'>
                {user?.firstname} {user?.lastname}
            </Paragraph>
            <Paragraph position='start' className="italic">
                {user?.title ?? 'New userNew'}
            </Paragraph>
        </div>
    </Header>
  );
}

export default Profile;