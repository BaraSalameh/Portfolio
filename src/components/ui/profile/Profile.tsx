// components/Header.tsx
import { Header } from '@/components/shared/Header';
import Image from 'next/image';
import React from 'react';
import { Paragraph } from '../Paragraph';
import { CUDModal } from '../CUDModal';
import { ProfileFormData } from '@/lib/schemas/profileSchema';
import { useParams } from 'next/navigation';
import ProfileForm from '@/app/[role]/[username]/forms/profileForm';

interface ProfileProps {
    user: ProfileFormData,
    className?: string;
}

const Profile = ({ 
    user,
    className
} : ProfileProps) => {

    const { role } = useParams<{role: 'owner' | 'client' | 'admin' }>();
    
    const profilePicture =
        user?.profilePicture ??
        (user?.gender === 0 ? '/Default-Female.svg' : '/Default-Male.svg');
    const coverPhoto = user?.coverPhoto ?? '/Default-CoverPhoto.svg';

  return (
    <Header space='lg' paddingY='sm' itemsY='start' className={`grid grid-cols-1 ${className}`}>
        <div className="relative h-35 sm:h-50 lg:h-60">
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
                {role === 'owner' &&
                    <div className='absolute right-0 bottom-0'>
                        <CUDModal as='update' subTitle='Update profile' >
                            <ProfileForm />
                        </CUDModal>
                    </div>
                }
            </div>
        </div>
        <div className='px-7 sm:px-10 lg:px-15 pt-3 space-y-1'>
            <Paragraph position='start' size='lg'>
                {user?.firstname} {user?.lastname}
            </Paragraph>
            <Paragraph position='start' className="italic">
                {user?.title}
            </Paragraph>
            {(user?.gender === 0 || user?.gender === 1 || user?.birthDate) && (
                <Paragraph position="start">
                    {user?.gender === 1
                    ? 'Male'
                    : user?.gender === 0
                    ? 'Female'
                    : ''}
                    {user?.birthDate ? ` (${user.birthDate})` : ''}
                </Paragraph>
            )}
            {(user?.bio) && 
                <hr className='pb-3' />
            }
            <Paragraph  className="italic">
                {user?.bio?.split('\n').map((line, index) => (
                    <span key={index}>
                        {line}
                    </span>
                ))}
            </Paragraph>
            
        </div>
    </Header>
  );
}

export default Profile;