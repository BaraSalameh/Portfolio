'use client';

import React from 'react';
import { anchor, AnchorVariantProps } from '@/styles/anchor';
import { cn } from '@/components/utils/cn'; // optional helper
import { useRouter } from 'next/navigation';

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

    const router = useRouter();
    return (
        <button onClick={() => url && router.push(url)} className={cn(anchor({ size }), className)}>
            {children}
        </button>
    );
};
