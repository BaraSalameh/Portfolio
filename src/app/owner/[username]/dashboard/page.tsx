'use client';

import { Header } from "@/components/shared/Header";
import { Main } from "@/components/shared/Main";
import { SubFooter } from "@/components/shared/SubFooter";
import { Anchor } from "@/components/ui/Anchor";
import { List } from "@/components/ui/List";
import { Paragraph } from "@/components/ui/Paragraph";
import { userByUsernameQuery } from "@/lib/apis/client/userBuUsernameQuery";
import { useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import { getDurationInYearsAndMonths } from "@/lib/utils/appFunctions";
import { useParams } from "next/navigation";
import { useEffect } from "react";

export default function OwnerDashboardPage() {

    const dispatch = useAppDispatch();
    const { user } = useAppSelector(state => state.user) as any;
    const { username } = useParams<{username: string}>(); 

    useEffect(() => {
        !user && dispatch(userByUsernameQuery(username));
    }, []);
    return (
        <>
        <Header itemsX='center'>
            <Paragraph size='xl' className="font-bold">Dashboard</Paragraph>
        </Header>
        <Main direction='row' className='flex-wrap'>
            {/* Education */}
            {Array.isArray(user?.lstEducations) && user.lstEducations.length > 0 &&
                <section className="bg-green-900 p-4 rounded-2xl">
                    <Paragraph size='lg' className="py-3">Education</Paragraph>
                    <List as='ul' size='sm'>
                        {user.lstEducations.map((edu: any, id: number) => 
                            <li key={id}>{edu.degree}. in {edu.fieldOfStudy} - {edu.institution}</li>
                        )}
                    </List>
                </section>
            }
            {/* Experience */}
            {Array.isArray(user?.lstExperiences) && user.lstExperiences.length > 0 &&
                <section className="bg-green-900 p-4 rounded-2xl">
                    <Paragraph size='lg' className="py-3">Experience</Paragraph>
                    <List as='ul' size='sm'>
                        {user.lstExperiences.map((exp: any, id: number) => 
                            <li key={id}>{exp.jobTitle} - {getDurationInYearsAndMonths(exp.startDate, exp.endDate)} ({exp.companyName})</li>
                        )}
                    </List>
                </section>
            }

            {/* Projects */}
            <section className="bg-white p-4 rounded-xl shadow-md">
                <h2 className="text-xl font-semibold mb-2">Projects</h2>
                <ul className="list-disc list-inside text-sm text-gray-700">
                    <li>Portfolio App with Next.js & ASP.NET API</li>
                    <li>Healthcare ZKP Research App (PhD project)</li>
                </ul>
            </section>

            {/* Skills */}
            <section className="bg-white p-4 rounded-xl shadow-md">
                <h2 className="text-xl font-semibold mb-2">Skills</h2>
                <p className="text-sm text-gray-700">React, Next.js, ASP.NET, Django, Spring Boot, SQL, Redux, Tailwind CSS, MediatR, ZKPs</p>
            </section>

            {/* Languages */}
            <section className="bg-white p-4 rounded-xl shadow-md">
                <h2 className="text-xl font-semibold mb-2">Languages</h2>
                <p className="text-sm text-gray-700">English (Fluent), Arabic (Native), Turkish (Intermediate)</p>
            </section>

            {/* Blog Posts */}
            <section className="bg-white p-4 rounded-xl shadow-md">
                <h2 className="text-xl font-semibold mb-2">Recent Blog Posts</h2>
                <ul className="list-disc list-inside text-sm text-gray-700">
                    <li>Zero-Knowledge Proofs in Healthcare</li>
                    <li>Best Practices for Securing ASP.NET APIs</li>
                </ul>
            </section>

            {/* Messages */}
            <section className="bg-white p-4 rounded-xl shadow-md col-span-1 md:col-span-2 xl:col-span-3">
                <h2 className="text-xl font-semibold mb-2">Messages</h2>
                <div className="space-y-2 text-sm text-gray-700">
                    <p><strong>HR, XYZ Corp:</strong> We’d love to schedule an interview with you.</p>
                    <p><strong>Dr. Smith:</strong> Please send the latest version of your ZKP implementation.</p>
                </div>
            </section>
        </Main>
        </>
    );
}
