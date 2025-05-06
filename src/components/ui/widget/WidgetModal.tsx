import { BlurBackGround } from '../BlurBackGround';
import { CUDModal } from '../CUDModal';
import { X } from 'lucide-react';
import { cn } from '@/components/utils/cn';
import { widgetCard } from '@/styles/widget';
import React from 'react';
import { Header } from '@/components/shared/Header';
import ResponsiveIcon from '../ResponsiveIcon';
import { ListItemConfig, WidgetList } from './WidgetList';
import { Main } from '@/components/shared/Main';
import { List } from '../List';

interface WidgetModalProps {
    isOpen: boolean;
    onClose: () => void;
    item?: Record<string, any>;
    update?: {
        title?: string;
        subTitle?: string;
        form?: React.ReactNode;
    };
    del?: {
        title?: string;
        subTitle?: string;
        message?: string;
        CBDelete: (id: string) => any;
    };
    details?: ListItemConfig[];
    className?: string;
}

export const WidgetModal: React.FC<WidgetModalProps> = ({ isOpen, onClose, item, update, del, details, className }) => {
    if (!isOpen) return null;

    return (
        <BlurBackGround intent='sm'>
            <div className={cn(widgetCard({}), className)}>
                <Header itemsX='between' paddingX="xs" paddingY="xs">
                    <div className='flex gap-3'>
                        {update && (
                            <CUDModal as='update' title={update.title} subTitle={update.subTitle}>
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
                            <CUDModal as='delete' title={del.title} subTitle={del.subTitle} CBRedux={del.CBDelete} idToDelete={item?.id}>
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
        </BlurBackGround>
    );
};
