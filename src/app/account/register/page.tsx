'use client';

import Image from "next/image";
import { Anchor } from "@/components/ui/Anchor";
import { Container } from "@/components/shared/Container";
import { Header } from "@/components/shared/Header";
import { Main } from "@/components/shared/Main";
import { SubFooter } from "@/components/shared/SubFooter";
import RegisterForm from "./RegisterForm";
import { useAppSelector } from "@/lib/store/hooks";
import { useEffect } from "react";
import { useRouter } from "next/navigation";

export default function RegisterPage() {

    var router = useRouter();
    const { username } = useAppSelector((state) => state.auth);

    useEffect(() => {
        username && router.push(`/account/register/${username}`);
    }, [username])

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
                    <RegisterForm />
                </div>
            </Main>
            <SubFooter>
                <Anchor size="xs" url="/">
                    <Image src="/file.svg" alt="File icon" width={16} height={16} />
                    Go home
                </Anchor>
                <Anchor size="xs" url="/account/login">
                    <Image src="/window.svg" alt="Window icon" width={16} height={16} />
                    I Do have an account! Login.
                </Anchor>
            </SubFooter>
        </Container>
    );
}
