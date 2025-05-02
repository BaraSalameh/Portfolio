'use client';

import LoginForm from "@/app/account/login/loginForm";
import {  useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import { useEffect } from "react";
import Image from "next/image";
import { Anchor } from "@/components/ui/Anchor";
import { Container } from "@/components/shared/Container";
import { Header } from "@/components/shared/Header";
import { Main } from "@/components/shared/Main";
import { SubFooter } from "@/components/shared/SubFooter";
import { useRouter } from "next/navigation";
import { validateToken } from "@/lib/apis/account/validateToken";

export default function LoginPage() {

    const router = useRouter();
    const { isConfirmed } = useAppSelector(state => state.auth);
    const dispatch = useAppDispatch();
    const { username } = useAppSelector(state => state.auth);

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
        <Container>
            <Header>
                <Image
                    src='/portfolio-logo.svg'
                    alt="portfolio logo"
                    width={180}
                    height={38}
                    priority
                />
            </Header>
            <Main>
                <div className="bg-green-950 rounded-2xl shadow-xl p-6 my-10">
                    <LoginForm />
                </div>
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
        </Container>
    );
}
