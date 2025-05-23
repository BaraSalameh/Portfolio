'use client';

import { Edit, LucideTrash2, X } from 'lucide-react';
import React, { useState } from 'react';
import { Button } from './form';
import Image from "next/image";
import { Header, Main } from '../shared';
import { cn } from '../utils';
import { widgetCard } from '@/styles/widget';
import { CUDProps } from './types';
import { ResponsiveIcon, BlurBackground, Paragraph } from '.';

export const CUDModal = ({
    isLoading,
    idToDelete,
    onAction,
    onClose,
    as = 'create',
    title,
    subTitle = title,
    children,
    icon,
    className
}: CUDProps) => {

    const [openModal, setOpenModal] = useState(false);
    const currentIcon =
        icon ? (
            <ResponsiveIcon icon={icon} />
        ) : as === 'create' ? (
            <ResponsiveIcon />
        ) : as === 'update' ? (
            <ResponsiveIcon icon={Edit} />
        ) : (
            <ResponsiveIcon icon={LucideTrash2} />
        );
    
    return (
        <>
        <div
            className="flex gap-2 cursor-pointer"
            onClick={() => setOpenModal(true)}
        >
            {currentIcon}
            {title && <Paragraph size='md'>{title}</Paragraph>}
        </div>
  
  
        {openModal && (
            <BlurBackground intent='sm'>
                <div className={ cn(widgetCard({ scroll: true }), className) }>
                    <Header itemsX="between" paddingX="xs" paddingY="xs">
                        {subTitle && <Paragraph size="md">{subTitle}</Paragraph>}
                        <ResponsiveIcon icon={X} onClick={() => setOpenModal(false)} className='cursor-pointer' />
                    </Header>
                    <hr />
                    <Main paddingX='sm' paddingY='sm' space='sm'>
                        {as !== 'delete'
                            ?   React.isValidElement(children)
                                    ?   React.cloneElement(children as React.ReactElement<{ onClose: () => void }>, {
                                            onClose: (children as any).props.onClose ?? (() => setOpenModal(false))
                                        })
                                    :   children
                            :   <>
                                <Paragraph size="md">{children}</Paragraph>
                                <Button
                                    onClick={async () => {
                                        if (onAction && idToDelete) {
                                            await onAction(idToDelete);
                                            onClose ? onClose() : setOpenModal(false);
                                            
                                        }
                                    }}
                                    intent="standard"
                                    rounded="full"
                                    size="lg"
                                    type="submit"
                                    disabled={isLoading}
                                >
                                    <Image
                                        className="dark:invert"
                                        src="/vercel.svg"
                                        alt="Vercel logomark"
                                        width={20}
                                        height={20}
                                    />
                                        {isLoading ? 'Deleting...' : 'Delete'}
                                </Button>
                                </>
                            }
                    </Main>
                </div>
            </BlurBackground>
        )}
        </>
    );
};
