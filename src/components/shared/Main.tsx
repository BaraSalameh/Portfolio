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
    className,
    direction,
    itemsX,
    itemsY,
    paddingX,
    paddingY,
    space

}) => {
    return (
        <div className={cn(main({ direction, itemsX, itemsY, paddingX, paddingY, space }), className)}>
            {children}
        </div>
    );
};
