'use client';

import React from 'react';
import { cn } from '@/components/utils/cn';
import { header, HeaderVariantProps } from '@/styles/header';

interface HeaderProps extends HeaderVariantProps {
    children: React.ReactNode;
    className?: string;
}

export const Header: React.FC<HeaderProps> = ({
    children,
    className,
    itemsX,
    itemsY,
    paddingX,
    paddingY,
    space

}) => {
    return (
        <div className={cn(header({ itemsX, itemsY, paddingX, paddingY, space}), className)}>
            {children}
        </div>
    );
};
