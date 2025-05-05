'use client';

import { Edit, LucideTrash2, X } from 'lucide-react';
import React, { InputHTMLAttributes, useState } from 'react';
import { Paragraph } from './Paragraph';
import { Button } from './Button';
import Image from "next/image";
import ResponsiveIcon from './ResponsiveIcon';
import { BlurBackGround } from './BlurBackGround';

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
            className="flex items-center justify-center gap-2 cursor-pointer"
            onClick={() => setOpenModal(true)}
        >
            {as === 'create'
            ?   <ResponsiveIcon />
            :   as === 'update'
                ?   <ResponsiveIcon icon={Edit} />
                :   <ResponsiveIcon icon={LucideTrash2} />
            }
            {title && <Paragraph size='md'>{title}</Paragraph>}
        </div>
  
        {openModal && (
            <BlurBackGround intent='sm'>
                <div className="flex flex-col bg-green-900 rounded-2xl gap-4 p-6 max-h-[85vh] overflow-auto scrollbar-hide">
                    <div className={headerStyle}>
                        {subTitle && <Paragraph size="md">{subTitle}</Paragraph>}
                        <X className='cursor-pointer' onClick={() => setOpenModal(false)} />
                    </div>
                    <hr />
                    {as !== 'delete'
                        ?   React.isValidElement(children)
                                ? React.cloneElement(children as React.ReactElement<{ onClose: () => void }>, {
                                        onClose: () => setOpenModal(false)
                                    })
                                : children
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
            </BlurBackGround>
        )}
        </>
    );
};
