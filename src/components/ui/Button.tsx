'use client';

import React from 'react';
import { button, ButtonVariantProps } from '@/styles/button';
import { cn } from '@/components/utils/cn'; // optional helper
import { useRouter } from 'next/navigation';

interface ButtonProps extends ButtonVariantProps {
    children: React.ReactNode;
    className?: string;
    type?: 'button' | 'submit' | 'reset';
    onClick?: () => void;
    url?: string;
    disabled?: boolean;
}

export const Button: React.FC<ButtonProps> = ({
    children,
    intent,
    size,
    rounded,
    className,
    type = 'button',
    onClick,
    url,
    disabled
}) => {
    const router = useRouter();
    
    const buttonContent = (

        <button
            type={type}
            className={cn(button({ intent, size, rounded }), className)}
            onClick={onClick}
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
