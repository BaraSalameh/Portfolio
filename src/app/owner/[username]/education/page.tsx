'use client';

import { Card } from "@/components/ui/Card";
import { CUDModal } from "@/components/ui/CUDModal";
import { useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import { useParams } from "next/navigation";
import EducationForm from "./educationForm";
import { useEffect } from "react";
import { educationListQuery } from "@/lib/apis/owner/educationListQuery";
import { Header } from "@/components/shared/Header";
import { Main } from "@/components/shared/Main";
import { deleteEducation } from "@/lib/apis/owner/deleteEducation";

export default function OwnerEducationPage() {

    const dispatch = useAppDispatch();
    const { educationList } = useAppSelector(state => state.education);
    const { username } = useParams<{username: string}>();

    useEffect(() => {
        educationList.length === 0 && dispatch(educationListQuery({username}));
    }, []);

    const handleDelete = async (id: string) => {
        try {
            await dispatch(deleteEducation(id)).unwrap();
            await dispatch(educationListQuery({ username }));
        } catch (err) {
            console.error('Failed to delete:', err);
        }
    };

    return (
        <>
        <Header itemsX='center'>
            <div className="p-4 dark:bg-green-900 rounded-2xl">
                <CUDModal as='create' title="Add Education">
                    <EducationForm />
                </CUDModal>
            </div>
        </Header>
        <Main direction='row' itemsY='start' className='flex-wrap'>
            {educationList?.map(edu => 
                <Card
                    key={edu.id}
                    institution={edu.institution}
                    degree={edu.degree}
                    fieldOfStudy={edu.fieldOfStudy}
                    startDate={edu.startDate}
                    endDate={edu.endDate}
                    description={edu.description}
                >
                    <CUDModal as='update' subTitle="Update Education">
                        <EducationForm educationId={edu.id} />
                    </CUDModal>
                    <CUDModal as='delete' subTitle="Delete Education" idToDelete={edu.id} CBRedux={handleDelete}>
                        Are you sure?
                    </CUDModal>
                </Card>
            )}
        </Main>
        </>
    );
}
