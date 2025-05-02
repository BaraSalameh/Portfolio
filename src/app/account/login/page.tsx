'use client';

import LoginForm from "@/app/account/login/loginForm";
import {  useAppSelector } from "@/lib/store/hooks";
import { useEffect } from "react";
import Image from "next/image";
import { Anchor } from "@/components/ui/Anchor";
import { Container } from "@/components/shared/Container";
import { Header } from "@/components/shared/Header";
import { Main } from "@/components/shared/Main";
import { SubFooter } from "@/components/shared/SubFooter";
import { useRouter } from "next/navigation";
import { useCheckAndGetUsername } from "@/lib/utils/appFunctions";

export default function LoginPage() {

    const router = useRouter();
    const getUsername = useCheckAndGetUsername();
    const { username, isConfirmed, loading } = useAppSelector(state => state.auth);

    useEffect(() => {
        const run = async () => {
            if(isConfirmed == false){
                router.push(`/account/register/confirm-email`);
                return;
            }

            const u = username || await getUsername();
            if (u) {
                router.push(`/owner/${u}/dashboard`);
            }
        };
        run();
    }, [username, isConfirmed]);

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
