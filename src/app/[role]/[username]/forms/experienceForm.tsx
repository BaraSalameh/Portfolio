'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { Paragraph } from "@/components/ui/Paragraph";
import { Button } from "@/components/ui/Button";
import Image from "next/image";
import { useForm, useWatch } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormInput } from "@/components/ui/FormInput";
import { List } from "@/components/ui/List";
import { FormCheckbox } from "@/components/ui/FormCheckbox";
import { useEffect } from "react";
import { ExperienceFormData, experienceSchema } from "@/lib/schemas/experienceSchema";
import { addEditExperience } from "@/lib/apis/owner/experience/addEditExperience";
import { experienceListQuery } from "@/lib/apis/owner/experience/experienceListQuery";
import { ExperienceProps } from "../types";

const ExperienceForm = ({id, onClose} : ExperienceProps) => {

    const dispatch = useAppDispatch();
    const { loading, error, lstExperiences } = useAppSelector((state) => state.experience);
    const experienceToHandle = lstExperiences.find(ex => ex.id === id);

    const {
        register,
        handleSubmit, 
        control,
        reset,
        formState: { errors },
    } = useForm<ExperienceFormData>({
        resolver: zodResolver(experienceSchema),
        defaultValues: {
            isWorking: false,
        }
    });

    const isWorking = useWatch({
        control,
        name: "isWorking",
        defaultValue: false,
    });

    const onSubmit = async (data: ExperienceFormData) => {
        await dispatch(addEditExperience(data));
        await dispatch(experienceListQuery());
        onClose?.();
    };

    useEffect(() => {
        if (experienceToHandle) {
            reset({...experienceToHandle});
        }
    }, [id]);

    return (
        <form onSubmit={handleSubmit(onSubmit)} className="relative space-y-4">
            <fieldset disabled={loading} className="space-y-4">

                <FormInput
                    label="Company"
                    type="text"
                    placeholder="e.g. Google"
                    registration={register('companyName')}
                    error={errors.companyName}
                />

                <FormInput
                    label="Job title"
                    type="text"
                    placeholder="e.g. Software Developer"
                    registration={register('jobTitle')}
                    error={errors.jobTitle}
                />
                
                <FormInput
                    label="Start date"
                    type="date"
                    registration={register('startDate')}
                    error={errors.startDate}
                />

                {!isWorking && (
                    <FormInput
                        label="End date"
                        type="date"
                        registration={register('endDate')}
                        error={errors.endDate}
                    />
                )}

                <FormCheckbox
                    label="Still working"
                    registration={register('isWorking')}
                    error={errors.isWorking}
                />

                <FormInput
                    label="Location"
                    type="text"
                    placeholder="e.g. The Champs-Élysées St - Paris"
                    registration={register('location')}
                    error={errors.location}
                />
                <FormInput
                    label="Description"
                    type="textarea"
                    placeholder="Description"
                    registration={register('description')}
                    error={errors.description}
                />
            </fieldset>
            

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

export default ExperienceForm;