'use client';

import { Edit, LucideTrash2, X, PlusIcon } from 'lucide-react';
import React, { InputHTMLAttributes, useState } from 'react';
import { Paragraph } from './Paragraph';
import { Button } from './Button';
import Image from "next/image";
import { useAppDispatch } from '@/lib/store/hooks';

interface CUDProps extends InputHTMLAttributes<HTMLInputElement> {
    idToDelete?: string;
    CBRedux?: (id: string) => any;
    as?: 'create' | 'update' | 'delete';
    title?: string;
    subTitle?: string;
    description?: string;
    label?: string;
    children?: React.ReactNode;
}

export const CUDModal = ({
    idToDelete,
    CBRedux,
    as = 'create',
    title,
    subTitle = title,
    description,
    label,
    children,
    ...rest
}: CUDProps) => {

    const [openModal, setOpenModal] = useState(false);
    const headerStyle = `flex justify-${subTitle ? 'between' : 'end'}`;
    
    return (
        <>
        <div
            className="flex gap-2 cursor-pointer"
            onClick={() => setOpenModal(true)}
        >
            {as === 'create'
            ?   <PlusIcon />
            :   as === 'update'
                ?   <Edit />
                :   <LucideTrash2 />
            }
            {title && <Paragraph size='md'>{title}</Paragraph>}
        </div>
  
        {openModal && (
            <div className="fixed inset-0 bg-black/30 backdrop-blur-md bg-opacity-50 flex items-center justify-center z-50">
                <div className="flex flex-col bg-green-900 rounded-2xl gap-4 p-6 max-h-[85vh] overflow-auto scrollbar-hide">
                    <div className={headerStyle}>
                        {subTitle && <Paragraph size="md">{subTitle}</Paragraph>}
                        <X className='cursor-pointer' onClick={() => setOpenModal(false)} />
                    </div>
                    <hr />
                    {as !== 'delete'
                        ?   children
                        :   <>
                            <Paragraph size="md">{children}</Paragraph>
                            <Button
                                onClick={async () => {
                                    if (CBRedux && idToDelete) {
                                        await CBRedux(idToDelete);
                                        setOpenModal(false);
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
                </div>
            </div>
        )}
        </>
    );
};
