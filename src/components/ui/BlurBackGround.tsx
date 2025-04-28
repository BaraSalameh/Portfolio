'use client';

import React from 'react';
import { cn } from '@/components/utils/cn'; // optional helper
import { blurBackGround, BlurBackGroundVariantProps } from '@/styles/blurBackGround';

interface BlurBackGroundProps extends BlurBackGroundVariantProps {
    children: React.ReactNode;
    className?: string;
    onClick?: () => void;
}

export const BlurBackGround: React.FC<BlurBackGroundProps> = ({
    children,
    intent,
    className,
    onClick
}) => {
    return (
        <div className={cn(blurBackGround({ intent }), className)} onClick={onClick}>
            {children}
        </div>
    );
};
