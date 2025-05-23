'use client';

import React from 'react';
import { cn } from '@/components/utils';
import { paragraph } from '@/styles';
import { ParagraphProps } from './types';

export const Paragraph = ({
    children,
    intent,
    size,
    text,
    position,
    space,
    className
}: ParagraphProps) => {
    return (
        <p className={cn(paragraph({ intent, size, text, position, space }), className)}>
            {children}
        </p>
    );

};
