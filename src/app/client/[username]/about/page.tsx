'use client';

import { Header } from "@/components/shared/Header";
import { Main } from "@/components/shared/Main";
import { Paragraph } from "@/components/ui/Paragraph";
import { userByUsernameQuery } from "@/lib/apis/client/userBuUsernameQuery";
import { useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import { useParams } from "next/navigation";
import { useEffect } from "react";

export default function ClientHomePage() {

    const dispatch = useAppDispatch();
    const { username } = useParams<{username: string}>();
    const { loading } = useAppSelector(state => state.client);
    
    useEffect(() => {
        username && dispatch(userByUsernameQuery(username));
    }, [username]);

    if (loading) {
        return (
            <Header itemsX='center'>
                <Paragraph>Loading...</Paragraph>
            </Header>
        );
    }
    
    return (
        <Main>
            <h1>Hello I am a client</h1>
        </Main>
    );
    
}
