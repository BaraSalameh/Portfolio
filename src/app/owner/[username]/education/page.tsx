'use client';

import { Card } from "@/components/ui/Card";
import { CUDModal } from "@/components/ui/CUDModal";
import { useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import EducationForm from "./educationForm";
import { useEffect } from "react";
import { educationListQuery } from "@/lib/apis/owner/educationListQuery";
import { Header } from "@/components/shared/Header";
import { Main } from "@/components/shared/Main";
import { deleteEducation } from "@/lib/apis/owner/deleteEducation";
import SortableEducationList from "@/components/ui/SortableList";

export default function OwnerEducationPage() {

    const dispatch = useAppDispatch();
    const { lstEducations } = useAppSelector(state => state.education);

    useEffect(() => {
        lstEducations.length === 0 && dispatch(educationListQuery());
    }, []);

    const handleDelete = async (id: string) => {
        try {
            await dispatch(deleteEducation(id));
            await dispatch(educationListQuery());
        } catch (err) {
            console.error('Failed to delete:', err);
        }
    };

    return (
        <>
        <Header itemsX='center'>
            <div className="flex items-center justify-center p-2 sm:p-4 dark:bg-green-900 rounded-2xl h-fit">
                <CUDModal as='create' title="Add Education">
                    <EducationForm />
                </CUDModal>
            </div>
        </Header>
        {lstEducations.length > 0 && 
            <Main>
                <SortableEducationList initialItems={lstEducations}/>
            </Main>
        }
        <Main direction='row' itemsY='start' className='flex-wrap h-fit'>
            {lstEducations?.map(edu => 
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
                        <EducationForm id={edu.id} />
                    </CUDModal>
                    <CUDModal as='delete' subTitle="Delete Education" idToDelete={edu.id} onAction={handleDelete}>
                        Are you sure?
                    </CUDModal>
                </Card>
            )}
        </Main>
        </>
    );
}
