'use client';

import React from 'react';
import { button } from '@/styles';
import { cn } from '@/components/utils';
import { useRouter } from 'next/navigation';
import { ButtonProps } from './types';

export const Button = ({
    children,
    intent,
    size,
    rounded,
    className,
    type = 'button',
    onClick,
    onClose,
    url,
    disabled
}: ButtonProps) => {
    const router = useRouter();

    const handleClick = () => {
        onClick?.();
        onClose?.();
    };
    
    const buttonContent = (

        <button
            type={type}
            className={cn(button({ intent, size, rounded }), className)}
            onClick={handleClick}
            disabled={disabled}
        >
            {children}
        </button>
    );

    if (url) {
        return (
            <button onClick={() => router.push(url)} className={cn(button({ intent, size }), className)}>
                {children}
            </button>
        );
    }

    return buttonContent;
};
