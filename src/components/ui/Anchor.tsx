'use client';

import React from 'react';
import { anchor, AnchorVariantProps } from '@/styles/anchor';
import { cn } from '@/components/utils/cn'; // optional helper

interface AnchorProps extends AnchorVariantProps {
    children: React.ReactNode;
    className?: string;
    type?: 'button' | 'submit' | 'reset';
    url?: string;
}

export const Anchor: React.FC<AnchorProps> = ({
    children,
    className,
    size,
    url
}) => {
    return (
        <a href={url} className={cn(anchor({ size }), className)}>
            {children}
        </a>
    );
};
