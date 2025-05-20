'use client';

import Image from "next/image";
import { Anchor } from "@/components/ui/Anchor";
import { Main } from "@/components/shared/Main";
import { SubFooter } from "@/components/shared/SubFooter";
import RegisterForm from "./RegisterForm";
import { useAppSelector } from "@/lib/store/hooks";
import { useEffect } from "react";
import { useRouter } from "next/navigation";
import { cn } from "@/components/utils/cn";
import { widgetCard } from "@/styles/widget";
import Loading from "@/components/shared/Loading";
import React from "react";

interface RegisterProps {
    className?: string;
}

const RegisterPage: React.FC<RegisterProps> = ({ className }) => {

    var router = useRouter();
    const { loading, username, isConfirmed } = useAppSelector((state) => state.auth);

    useEffect(() => {
        username && isConfirmed === false && router.push(`/account/register/confirm-email`);
    }, [username])

    return (
        <React.Fragment>
            <Loading isLoading={loading} />
            <Main paddingY='none'>
                <section className={cn(widgetCard({ scroll: true }), className)}>
                    <RegisterForm />
                </section>
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
        </React.Fragment>
    );
}

export default RegisterPage;
