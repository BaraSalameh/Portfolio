'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { Paragraph } from "@/components/ui/Paragraph";
import { Button } from "@/components/ui/Button";
import Image from "next/image";
import { Controller, useForm, useWatch } from "react-hook-form";
import { EducationFormData, educationSchema } from "@/lib/schemas/educationSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormInput } from "@/components/ui/FormInput";
import { List } from "@/components/ui/List";
import { FormCheckbox } from "@/components/ui/FormCheckbox";
import { addEditEducation } from "@/lib/apis/owner/addEditEducation";
import { educationListQuery } from "@/lib/apis/owner/educationListQuery";
import { useEffect, useMemo } from "react";
import { institutionListQuery } from "@/lib/apis/owner/education/institutionListQuery";
import { FormDropdown } from "@/components/ui/FormDropdown";
import { degreeListQuery } from "@/lib/apis/owner/education/degreeListQuery";
import { fieldOfStudyListQuery } from "@/lib/apis/owner/education/fieldOfStudyListQuery";
import { getSelectedOption, mapEducationToForm } from "@/lib/utils/appFunctions";
import { EducationProps } from "../../types";

export default function EducationForm({id, onClose} : EducationProps) {

    const dispatch = useAppDispatch();
    const { loading, error } = useAppSelector((state) => state.auth);
    const { lstEducations, lstInstitutions, lstDegrees, lstFields } = useAppSelector((state) => state.education);
    const educationToHandle = lstEducations.find(ed => ed.id === id);

    const institutionOptions = useMemo(() =>
        lstInstitutions.map(i => ({ label: i.name, value: i.id }))
    , [lstInstitutions]);

    const degreeOptions = useMemo(() =>
        lstDegrees.map(i => ({ label: i.name, value: i.id }))
    , [lstDegrees]);

    const fieldOptions = useMemo(() =>
        lstFields.map(i => ({ label: i.name, value: i.id }))
    , [lstFields]);

    const {
        register,
        handleSubmit, 
        control,
        reset,
        formState: { errors },
    } = useForm<EducationFormData>({
        resolver: zodResolver(educationSchema),
        defaultValues: {
            isStudying: false,
        }
    });

    const isStudying = useWatch({
        control,
        name: "isStudying",
        defaultValue: false,
    });

    const onSubmit = async (data: EducationFormData) => {
        await dispatch(addEditEducation(data));
        await dispatch(educationListQuery());
        onClose?.();
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
    }, []);
      
    return (
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">

            <Controller
                name="LKP_InstitutionID"
                control={control}
                render={({ field }) => (
                    <FormDropdown
                        label="Institution"
                        options={institutionOptions}
                        value={getSelectedOption(institutionOptions, field.value)}
                        onChange={(option) => field.onChange(option?.value ?? '')}
                        onBlur={field.onBlur}
                        error={errors.LKP_InstitutionID}
                    />
                )}
            />

            <Controller
                name="LKP_DegreeID"
                control={control}
                render={({ field }) => (
                    <FormDropdown
                        label="Degree"
                        options={degreeOptions}
                        value={getSelectedOption(degreeOptions, field.value)}
                        onChange={(option) => field.onChange(option?.value ?? '')}
                        onBlur={field.onBlur}
                        error={errors.LKP_DegreeID}
                    />
                )}
            />

            <Controller
                name="LKP_FieldOfStudyID"
                control={control}
                render={({ field }) => (
                    <FormDropdown
                        label="Field of study"
                        options={fieldOptions}
                        value={getSelectedOption(fieldOptions, field.value)}
                        onChange={(option) => field.onChange(option?.value ?? '')}
                        onBlur={field.onBlur}
                        error={errors.LKP_FieldOfStudyID}
                    />
                )}
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
