'use client';

import { useAppSelector } from "@/lib/store/hooks";
import { useLoadContactMessageData } from "./handlers";
import { useContactMessageWidget } from "./hooks";
import { ControlledWidget, Paragraph } from "@/components";
import React from "react";

export const ContactMessagePage = () => {

    const { lstMessages } = useAppSelector(state => state.contactMessage);
    useLoadContactMessageData(lstMessages);
    const contactMessageWidget = useContactMessageWidget();

    return (
        lstMessages.length > 0
            ?   <ControlledWidget
                    {...contactMessageWidget}
                />
            :   <Paragraph size='sm'>{"No messages found"}</Paragraph>
    )
}