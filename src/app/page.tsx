'use client'
import { Button, Anchor } from "@/components/ui/form";
import { SearchBar } from "@/components/ui";
import Image from "next/image";
import { useState } from 'react';
import { abstractParagraph, listParagraphOne, listParagrapTwo } from "@/lib/constants";
import { Paragraph, List } from "@/components/ui";
import { Container } from "@/components/shared/Container";
import { Header } from "@/components/shared/Header";
import { Main } from "@/components/shared/Main";
import { SubFooter } from "@/components/shared/SubFooter";
import { BlurBackground } from "@/components/ui";

export default function Home() {
    const [searchOpen, setSearchOpen] = useState(false);

    const handleFocus = () => {
        setSearchOpen(true);
    };

    const handleClose = () => {
        setSearchOpen(false);
    };

    return (
        <>
           {searchOpen && (
                <BlurBackground intent='sm'>
                    {/* Backdrop */}
                    <div
                        className="absolute inset-0 bg-black/30 backdrop-blur-sm cursor-pointer"
                        onClick={handleClose}
                    />

                    {/* SearchBar */}
                    <div
                        onClick={(e) => e.stopPropagation()}
                        className="z-10 sm:w-full max-w-md"
                    >
                        <SearchBar />
                    </div>
                </BlurBackground>
            )}
            {/* Main content */}
            <Container>
                <Header>
                    <Image
                        src='/portfolio-logo.svg'
                        alt="portfolio logo"
                        width={300}
                        height={40}
                        priority
                    />
                </Header>
                <Main className='sm:items-start items-center'>
                    <Paragraph size="md" text='justify'>
                        {abstractParagraph}
                    </Paragraph>
                    <List size="sm">
                        <li>
                            {listParagraphOne}
                        </li>
                        <li>
                            {listParagrapTwo}
                        </li>
                    </List>
                    <div className="flex gap-4 items-center flex-col sm:flex-row">
                        <Button intent="standard" size="lg" rounded="full" onClick={handleFocus}>
                            <Image
                                className="dark:invert"
                                src="/vercel.svg"
                                alt="Vercel logomark"
                                width={20}
                                height={20}
                            />
                            Start searching
                        </Button>
                        <Button url="/account/login" size="lg" rounded="full">Login</Button>
                        <Button url="/account/register" size="lg" rounded="full">Register</Button>
                    </div>
                </Main>
                <SubFooter>
                    <Anchor size="xs" url="#">
                        <Image src="/file.svg" alt="File icon" width={16} height={16} />
                        More details
                    </Anchor>
                    <Anchor size="xs" url="#">
                        <Image src="/window.svg" alt="Window icon" width={16} height={16} />
                        Examples
                    </Anchor>
                    <Anchor size="xs" url="#">
                        <Image src="/globe.svg" alt="Globe icon" width={16} height={16} />
                        Contact us →
                    </Anchor>
                </SubFooter>
            </Container>
        </>
    );
}
