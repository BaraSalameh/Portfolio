'use client';

import LoginForm from "@/app/account/login/loginForm";
import { Card } from "@/components/ui/Card";
import { CUDModal } from "@/components/ui/CUDModal";
import { useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import { useParams, useRouter } from "next/navigation";
import EducationForm from "./educationForm";
import { useEffect } from "react";
import { educationListQuery } from "@/lib/apis/owner/educationListQuery";

export default function OwnerEducationPage() {

    const dispatch = useAppDispatch();
    const { education } = useAppSelector(state => state.education);
    const { username } = useParams<{username: string}>();

    useEffect(() => {
        education.length === 0 && dispatch(educationListQuery({username}));
    }, []);

    return (
        <div className="flex flex-col gap-3 items-center p-10 sm:px-20">
            <div className="p-4 dark:bg-green-900 rounded-2xl">
                <CUDModal as='create' title="Add Education">
                    <EducationForm />
                </CUDModal>
            </div>
            <div className="flex flex-wrap justify-center gap-3" >
                {education?.map((edu : any) => 
                    <Card key={edu.id} title={edu.institution} description="Computer science" label="Learn more">
                        <CUDModal as='update' subTitle="Update Education">
                            <EducationForm />
                        </CUDModal>
                        <CUDModal as='delete' subTitle="Delete Education">
                            <EducationForm />
                        </CUDModal>
                    </Card>
                )}
            </div>
        </div>
    );
}
