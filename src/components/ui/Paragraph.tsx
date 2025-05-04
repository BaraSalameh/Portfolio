'use client';

import React from 'react';
import { cn } from '@/components/utils/cn';
import { paragraph, ParagraphVariantProps } from '@/styles/paragraph';

interface ParagraphProps extends ParagraphVariantProps {
    children: React.ReactNode;
    className?: string;
}

export const Paragraph: React.FC<ParagraphProps> = ({
    children,
    intent,
    size,
    text,
    position,
    className
}) => {
    return (
        <p className={cn(paragraph({ intent, size, text, position }), className)}>
            {children}
        </p>
    );

};
