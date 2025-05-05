import {Cell, Tooltip, BarChart, Bar, XAxis, YAxis, ResponsiveContainer } from 'recharts';
import { Briefcase, Clock, MapPin } from 'lucide-react';
import dayjs from 'dayjs';
import { Paragraph } from './Paragraph';
import ResponsiveIcon from './ResponsiveIcon';
import { Header } from '../shared/Header';
import { Main } from '../shared/Main';
import { List } from './List';
import { PieChartWidget } from './widget/PieChart';
import { BarChartWidget } from './widget/BarChart';
import { generateColorMap, generateDurationData, generatePieData } from '@/lib/utils/appFunctions';

export function ExperienceCard({ lstExperiences }: { lstExperiences: any }) {
    if (!Array.isArray(lstExperiences) || lstExperiences.length === 0) return null;

    const pieData = generatePieData(lstExperiences, 'jobTitle');
    const durationData = generateDurationData(lstExperiences, 'jobTitle');
    const colorMap = generateColorMap(pieData);
    
    return (
        <section className="bg-green-900 p-4 rounded-2xl">
            <Header paddingX='xs' paddingY='xs' space='sm'>
                <ResponsiveIcon icon={Briefcase} />
                <Paragraph size='lg'>Experience</Paragraph>
            </Header>

            <Main paddingX='none' paddingY='md'>
                <div className="grid grid-cols-1 sm:grid-cols-3 w-full">
                    <div className="h-64">
                        <Paragraph size='md' position='center'>Job Titles Overview</Paragraph>
                        <PieChartWidget data={pieData} />
                    </div>
                    <div className="h-64 col-span-2 space-y-3">
                        <Paragraph size='md' position='center'>Duration (in months)</Paragraph>
                        <BarChartWidget data={durationData} colorMap={colorMap} />
                    </div>
                </div>
            </Main>

            {/* List of Experiences */}
            {/* <Main paddingX='none' paddingY='none'>
                <List size='md' as='none' className='w-full'>
                    {lstExperiences.map(exp => (
                        <li key={exp.id} className="bg-green-700 p-4 rounded-xl shadow-sm space-y-2">
                            <Paragraph size='lg'>{exp.jobTitle} at {exp.companyName}</Paragraph>
                            <Paragraph className='text-green-200 flex gap-2 items-center'>
                                <ResponsiveIcon icon={MapPin} />
                                {exp.location}
                            </Paragraph>
                            <Paragraph className='text-green-200 flex gap-2 items-center'>
                                <ResponsiveIcon icon={Clock} />
                                {dayjs(exp.startDate).format('MMM YYYY')} – {exp.endDate ? dayjs(exp.endDate).format('MMM YYYY') : 'Present'}
                            </Paragraph>
                        </li>
                    ))}
                </List>
            </Main> */}
        </section>
    );
}
