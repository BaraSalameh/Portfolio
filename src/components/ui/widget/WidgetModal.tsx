import { CUDModal, BlurBackground, ResponsiveIcon, List } from '..';
import { X } from 'lucide-react';
import { cn } from '@/components/utils';
import { widgetCard } from '@/styles';
import React from 'react';
import { Header, Main } from '@/components/shared';
import { WidgetList } from './WidgetList';
import { WidgetModalProps } from './types';

export const WidgetModal = ({ isLoading, isOpen, onClose, onAction, item, update, del, details, className }: WidgetModalProps) => {
    if (!isOpen) return null;

    onAction?.(item?.id);
    
    return (
        <BlurBackground intent='sm' className='p-5'>
            <div className={cn(widgetCard({}), className)}>
                <Header itemsX='between' paddingX="xs" paddingY="xs">
                    <div className='flex gap-3'>
                        {update && (
                            <CUDModal isLoading={isLoading} as='update' title={update.title} subTitle={update.subTitle}>
                                {React.isValidElement(update.form)
                                    ?   React.cloneElement(update.form as React.ReactElement<{ onClose: () => void; id: string }>, {
                                            onClose,
                                            id: item?.id,
                                        })
                                    :   update.form
                                }
                            </CUDModal>
                        )}
                        {del && (
                            <CUDModal isLoading={isLoading} as='delete' title={del.title} subTitle={del.subTitle} onAction={del.onDelete} onClose={onClose} idToDelete={item?.id}>
                                {del.message}
                            </CUDModal>
                        )}
                    </div>
                    <ResponsiveIcon icon={X} onClick={onClose} className='cursor-pointer' />
                </Header>
                {details &&
                    <Main paddingX="none" paddingY="none">
                        <List size="md" as="none" className="w-full">
                            <WidgetList items={[item ?? {}]} list={details}  />
                        </List>
                    </Main>
                }
            </div>
        </BlurBackground>
    );
};
