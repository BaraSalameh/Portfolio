'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { Paragraph } from "@/components/ui/Paragraph";
import { Button } from "@/components/ui/Button";
import Image from "next/image";
import { useForm, useWatch } from "react-hook-form";
import { EducationFormData, educationSchema } from "@/lib/schemas/educationSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormInput } from "@/components/ui/FormInput";
import { List } from "@/components/ui/List";
import { FormCheckbox } from "@/components/ui/FormCheckbox";
import { addEditEducation } from "@/lib/apis/owner/addEditEducation";
import { educationListQuery } from "@/lib/apis/owner/educationListQuery";
import { useEffect, useState } from "react";
import { institutionListQuery } from "@/lib/apis/owner/education/institutionListQuery";
import { FormDropdown } from "@/components/ui/FormDropdown";
import { degreeListQuery } from "@/lib/apis/owner/education/degreeListQuery";
import { fieldOfStudyListQuery } from "@/lib/apis/owner/education/fieldOfStudyListQuery";
import { mapEducationToForm } from "@/lib/utils/appFunctions";

type props = {
    id?: string;
    onClose?: () => void;
}

export default function EducationForm({id, onClose} : props) {

    const dispatch = useAppDispatch();
    const { loading, error } = useAppSelector((state) => state.auth);
    const { lstEducations, lstInstitutions, lstDegrees, lstFields } = useAppSelector((state) => state.education);
    const educationToHandle = lstEducations.find(ed => ed.id === id);

    const institutionOptions = lstInstitutions.map(i => ({
        label: i.name,
        value: i.id
    }));

    const degreeOptions = lstDegrees.map(i => ({
        label: i.name,
        value: i.id
    }));

    const fieldOptions = lstFields.map(i => ({
        label: i.name,
        value: i.id
    }));

    const {
        register,
        handleSubmit, 
        control,
        setValue,
        watch,
        reset,
        formState: { errors },
    } = useForm<EducationFormData>({
        resolver: zodResolver(educationSchema),
        defaultValues: {
            isStudying: false, // Important: set default
        }
    });

    const isStudying = useWatch({
        control,
        name: "isStudying",
        defaultValue: false,
    });

    const onSubmit = async (data: EducationFormData) => {
        try {
            await dispatch(addEditEducation(data));
            await dispatch(educationListQuery());
            onClose?.();
        } catch (error) {
            // console.log('Error creating education: ', error);
        }
    };

    useEffect(() => {
        if (educationToHandle) {
            reset(mapEducationToForm(educationToHandle));
        }
    }, [id]);

    useEffect(() => {
        lstInstitutions.length === 0 && dispatch(institutionListQuery());
        lstDegrees.length === 0 && dispatch(degreeListQuery());
        lstFields.length === 0 && dispatch(fieldOfStudyListQuery());
        console.log(watch('LKP_InstitutionID'));
    }, []);
      
    return (
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <FormDropdown
                label="Institution"
                options={institutionOptions}
                value={institutionOptions.find(opt => opt.value === watch('LKP_InstitutionID'))}
                onChange={(option) => setValue('LKP_InstitutionID', option?.value as string)}
                error={errors.LKP_InstitutionID}
            />
            
            <FormDropdown
                label="Degree"
                options={degreeOptions}
                value={degreeOptions.find(opt => opt.value === watch('LKP_DegreeID'))}
                onChange={(option) => setValue('LKP_DegreeID', option?.value as string)}
                error={errors.LKP_DegreeID}
            />

            <FormDropdown
                label="Field of study"
                options={fieldOptions}
                value={fieldOptions.find(opt => opt.value === watch('LKP_FieldOfStudyID'))}
                onChange={(option) => setValue('LKP_FieldOfStudyID', option?.value as string)}
                error={errors.LKP_FieldOfStudyID}
            />

            <FormInput
                label="Start date"
                type="date"
                registration={register('startDate')}
                error={errors.startDate}
            />

            {!isStudying && (
                <FormInput
                    label="End date"
                    type="date"
                    registration={register('endDate')}
                    error={errors.endDate}
                />
            )}

            <FormCheckbox
                label="Still studying"
                registration={register('isStudying')}
                error={errors.isStudying}
            />

            <FormInput
                label="Description"
                type="text"
                placeholder="Desciption"
                registration={register('description')}
                error={errors.description}
            />

            {Array.isArray(error) && error.length > 1 ? (
                <List intent="danger" size="sm">
                    {error.map((e: string, i: number) => (
                        <li key={i}>{e}</li>
                    ))}
                </List>
            ) : (
                error && <Paragraph intent="danger" size="sm">{error}</Paragraph>
            )}

            <Button intent="standard" rounded="full" size="lg" type="submit" disabled={loading}>
                <Image
                    className="dark:invert"
                    src="/vercel.svg"
                    alt="Vercel logomark"
                    width={20}
                    height={20}
                />
                {loading ? 'Submitting...' : 'Submit'}
            </Button>
        </form>
    );
}
