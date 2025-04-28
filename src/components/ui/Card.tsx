'use client';

import { X } from 'lucide-react';
import { Children, InputHTMLAttributes, useState } from 'react';
import { Paragraph } from './Paragraph';
import { CUDModal } from './CUDModal';

interface CardProps extends InputHTMLAttributes<HTMLInputElement> {
    title?: string;
    description?: string;
    label?: string;
    children?: React.ReactNode;
}

export const Card = ({
    title,
    description,
    label,
    children,
    ...rest
}: CardProps) => {

    const [openModal, setOpenModal] = useState(false);
    const headerStyle = `flex justify-${children ? 'between' : 'end'}`;

    return (
        <>
        <div
            className="flex p-4 rounded-2xl bg-green-900 cursor-pointer"
            onClick={() => setOpenModal(true)}
        >
          <div className='flex flex-col gap-2 w-5xs sm:w-3xs'>
                <Paragraph size='sm'>{title ?? 'Title' }</Paragraph>
                <Paragraph size="xs">{description ?? 'Description'}</Paragraph>
          </div>
        </div>
  
        {openModal && (
            <div className="fixed inset-0 bg-black/50 backdrop-blur-md bg-opacity-50 flex items-center justify-center z-50">
                <div className="flex flex-col bg-green-900 rounded-2xl gap-6 p-8">
                    <div className={headerStyle}>
                        <div className='flex gap-5'>
                            {children}
                        </div>
                        <X className='cursor-pointer' onClick={() => setOpenModal(false)} />
                    </div>
                    <div className='flex flex-col gap-4'>
                        <Paragraph size="md">{title}</Paragraph>
                        <Paragraph size="xs">{description}</Paragraph>
                    </div>
                </div>
            </div>
        )}
        </>
    );
};
