'use client';

import LoginForm from "@/app/account/forms/loginForm";
import {  useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import { useEffect } from "react";
import Image from "next/image";
import { Anchor } from "@/components/ui/form/Anchor";
import { useRouter } from "next/navigation";
import { validateToken } from "@/lib/apis/account/validateToken";
import { cn } from "@/components/utils/cn";
import { widgetCard } from "@/styles/widget";
import React from "react";
import { Loading, Main, SubFooter } from "@/components/shared";

const LoginPage = () => {

    const router = useRouter();
    const { loading, isConfirmed, username } = useAppSelector(state => state.auth);
    const dispatch = useAppDispatch();

    useEffect(() => {
        const checkSession = async () => {
            !username && await dispatch(validateToken());
            if (username){
                router.push(`/owner/${username}/dashboard`);
            }
        }
        checkSession();
    }, [username]);

    useEffect(() => {
        if(isConfirmed === false){
            router.push(`/account/register/confirm-email`);
            return;
        }
    }, [isConfirmed]);

    return (
        <React.Fragment>
            <Loading isLoading={loading} />
            <Main>
                <section className={cn(widgetCard())}>
                    <LoginForm />
                </section>
            </Main>
            <SubFooter>
                <Anchor size="xs" url="/">
                    <Image src="/file.svg" alt="File icon" width={16} height={16} />
                    Go home
                </Anchor>
                <Anchor size="xs" url="/account/register">
                    <Image src="/window.svg" alt="Window icon" width={16} height={16} />
                    Don't have an account? Register!
                </Anchor>
            </SubFooter>
        </React.Fragment>
    );
}

export default LoginPage;
