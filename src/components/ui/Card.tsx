'use client';

import { X } from 'lucide-react';
import { useState } from 'react';
import { Paragraph } from './Paragraph';
import { card, CardVariantProps } from '@/styles/card';
import { cn } from '../utils/cn';
import { BlurBackGround } from './BlurBackGround';

interface CardProps extends CardVariantProps {
    institution?: string;
    degree?: string;
    fieldOfStudy?: string;
    startDate?: string;
    endDate?: string;
    description?: string;
    children?: React.ReactNode;
    className?: string;
}

export const Card: React.FC<CardProps> = ({
    institution,
    degree,
    fieldOfStudy,
    startDate,
    endDate,
    description,
    children,
    intent,
    className,
    size,
    rounded,
}) => {

    const [openModal, setOpenModal] = useState(false);
    const headerStyle = `flex justify-${children ? 'between' : 'end'}`;

    return (
        <>
        <div
            className={`${cn(card({ intent, size, rounded }), className)}`}
            onClick={() => setOpenModal(true)}
        >
          <div className='flex flex-col gap-2'>
                <Paragraph size='lg'>{institution}</Paragraph>
                <Paragraph size="md">{degree} - {fieldOfStudy}</Paragraph>
          </div>
        </div>
  
        {openModal && (
            <BlurBackGround intent='sm'>
                <div className="flex flex-col bg-green-900 rounded-2xl gap-6 p-8">
                    <div className={headerStyle}>
                        <div className='flex gap-5'>
                            {children}
                        </div>
                        <X className='cursor-pointer' onClick={() => setOpenModal(false)} />
                    </div>
                    <hr />
                    <div className='flex flex-col gap-4'>
                        <Paragraph size="lg">{institution}</Paragraph>
                        <Paragraph size="md">{degree} - {fieldOfStudy}</Paragraph>
                        <Paragraph size="sm"> {startDate} - {endDate ?? 'Now'}</Paragraph>
                        <Paragraph size="sm"> {description}</Paragraph>
                    </div>
                </div>
            </BlurBackGround>
        )}
        </>
    );
};
