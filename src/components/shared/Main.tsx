'use client';

import React from 'react';
import { cn } from '@/components/utils/cn';
import { main, MainVariantProps } from '@/styles/main';

interface MainProps extends MainVariantProps {
    children: React.ReactNode;
    className?: string;
}

export const Main: React.FC<MainProps> = ({
    children,
    className

}) => {
    return (
        <div className={cn(main(), className)}>
            {children}
        </div>
    );
};
