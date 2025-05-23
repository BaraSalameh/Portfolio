'use client';

import Image from "next/image";
import { Anchor } from "@/components/ui/form/Anchor";
import { Main } from "@/components/shared/Main";
import { SubFooter } from "@/components/shared/SubFooter";
import { Paragraph } from "@/components/ui/Paragraph";
import React from "react";

export default function ConfirmEmailPage() {
    return (
        <React.Fragment>
            <Main>
                <div className="bg-green-950 rounded-2xl shadow-xl p-6 my-10">
                    <Paragraph size="md">
                        A Confirmation request has been sent to your email!
                        Please check your inbox
                    </Paragraph>
                </div>
            </Main>
            <SubFooter>
                <Anchor size="xs" url="/">
                    <Image src="/file.svg" alt="File icon" width={16} height={16} />
                    Go home
                </Anchor>
            </SubFooter>
        </React.Fragment>
    );
}
