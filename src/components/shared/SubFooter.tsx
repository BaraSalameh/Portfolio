'use client';

import React from 'react';
import { cn } from '@/components/utils/cn';
import { subFooter, SubFooterVariantProps } from '@/styles/subFooter';

interface SubFooterProps extends SubFooterVariantProps {
    children: React.ReactNode;
    className?: string;
}

export const SubFooter: React.FC<SubFooterProps> = ({
    children,
    className

}) => {
    return (
        <div className={cn(subFooter(), className)}>
            {children}
        </div>
    );
};
