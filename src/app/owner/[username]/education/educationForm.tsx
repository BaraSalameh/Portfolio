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
import { useEffect } from "react";

type props = {
    educationId?: string;
    onClose?: () => void;
}

export default function EducationForm({educationId, onClose} : props) {

    const dispatch = useAppDispatch();
    const { loading, error } = useAppSelector((state) => state.auth);
    const { educationList } = useAppSelector((state) => state.education);
    const educationToHandle = educationList.find(ed => ed.id === educationId);

    const {
        register,
        handleSubmit, 
        control,
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
            await dispatch(addEditEducation(data)).unwrap();
            await dispatch(educationListQuery());
            onClose?.();
        } catch (error) {
            // console.log('Error creating education: ', error);
        }
    };

    useEffect(() => {
        if (educationToHandle) {
            reset({
                ...educationToHandle,
                startDate: educationToHandle.startDate?.slice(0, 10),
                endDate: educationToHandle.endDate?.slice(0, 10),
            });
        }
    }, [educationId]);
      
    return (
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <FormInput
                label="Institution"
                type="text"
                placeholder="Harvard University"
                registration={register('institution')}
                error={errors.institution}
            />
            <FormInput
                label="Degree"
                type="text"
                placeholder="Bachelor"
                registration={register('degree')}
                error={errors.degree}
            />
            <FormInput
                label="Field of study"
                type="text"
                placeholder="Computer Science"
                registration={register('fieldOfStudy')}
                error={errors.fieldOfStudy}
            />
            <FormInput
                label="Start date"
                type="date"
                registration={register('startDate')}
                error={errors.startDate}
            />

            {/* Only show endDate if not studying */}
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
