'use client';

import React from 'react';
import { button, ButtonVariantProps } from '@/styles/button';
import { cn } from '@/components/utils/cn'; // optional helper

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
            <a href={url} className={cn(button({ intent, size, rounded }), className)}>
                {children}
            </a>
        );
    }

    return buttonContent;
};
