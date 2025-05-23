'use client';

import React from 'react';
import { anchor } from '@/styles';
import { cn } from '@/components/utils';
import { useRouter } from 'next/navigation';
import { AnchorProps } from './types';

export const Anchor = ({
    className,
    size,
    url,
    children
}: AnchorProps) => {
    const router = useRouter();
    
    return (
        <button onClick={() => url && router.push(url)} className={cn(anchor({ size }), className)}>
            {children}
        </button>
    );
};
