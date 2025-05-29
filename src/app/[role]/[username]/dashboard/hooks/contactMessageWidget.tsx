import { WidgetCardProps } from "@/components/ui/widget/types";
import { useAppSelector } from "@/lib/store/hooks";
import {  MessageCircleHeart, User, SubtitlesIcon, Mail } from "lucide-react";
import { useHandleMessageDelete, useHandleSignMessage } from "../handlers";
import { ContactMessagePage } from "../ContactMessagePage";

export const useContactMessageWidget = (): WidgetCardProps => {

    const { loading, lstMessages } = useAppSelector(state => state.contactMessage);

    const handleMessageDelete = useHandleMessageDelete();
    const handleSignMessage = useHandleSignMessage();

    return {
        isLoading: loading,
        items: lstMessages,
        list: [
            { leftKey: 'name', size: 'lg', icon: User },
            { leftKey: 'email', icon: Mail },
            { leftKey: 'subject', icon: SubtitlesIcon },
        ],
        details: [
            { leftKey: 'name', size: 'lg', icon: User },
            { leftKey: 'email', icon: Mail },
            { leftKey: 'subject', icon: SubtitlesIcon },
            { leftKey: 'message', icon: MessageCircleHeart },
        ],
        del: { subTitle: 'Delete Message', message: 'Are you sure?', onDelete: handleMessageDelete },
        onModalAction: handleSignMessage
    }
}