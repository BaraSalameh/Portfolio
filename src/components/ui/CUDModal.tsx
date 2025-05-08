'use client';

import { Edit, LucideTrash2, X } from 'lucide-react';
import React, { InputHTMLAttributes, useState } from 'react';
import { Paragraph } from './Paragraph';
import { Button } from './Button';
import Image from "next/image";
import ResponsiveIcon from './ResponsiveIcon';
import { BlurBackGround } from './BlurBackGround';
import { Header } from '../shared/Header';
import { cn } from '../utils/cn';
import { widgetCard } from '@/styles/widget';
import { Main } from '../shared/Main';

interface CUDProps extends InputHTMLAttributes<HTMLInputElement> {
    idToDelete?: string;
    onAction?: (id: string) => any;
    onClose?: () => void;
    as?: 'create' | 'update' | 'delete' | 'none';
    title?: string;
    subTitle?: string;
    children?: React.ReactNode;
    className?: string;
}

export const CUDModal = ({
    idToDelete,
    onAction,
    onClose,
    as = 'create',
    title,
    subTitle = title,
    children,
    className
}: CUDProps) => {

    const [openModal, setOpenModal] = useState(false);
    
    return (
        <>
        <div
            className="flex items-center justify-center gap-2 cursor-pointer"
            onClick={() => setOpenModal(true)}
        >
            {as === 'create'
            ?   <ResponsiveIcon />
            :   as === 'update'
                ?   <ResponsiveIcon icon={Edit} />
                :  <ResponsiveIcon icon={LucideTrash2} />
            }
            {title && <Paragraph size='md'>{title}</Paragraph>}
        </div>
  
  
        {openModal && (
            <BlurBackGround intent='sm'>
                <div className={ cn(widgetCard({}), className) }>
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
                                >
                                    <Image
                                        className="dark:invert"
                                        src="/vercel.svg"
                                        alt="Vercel logomark"
                                        width={20}
                                        height={20}
                                    />
                                        Delete
                                </Button>
                                </>
                            }
                    </Main>
                </div>
            </BlurBackGround>
        )}
        </>
    );
};
