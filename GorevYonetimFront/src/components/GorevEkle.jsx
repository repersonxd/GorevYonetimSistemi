import React, { useState } from 'react';
import { useTasks } from '../contexts/TaskContext';
import { Form, Input, Button, message } from 'antd';

function GorevEkle() {
    const [form] = Form.useForm();
    const { addTask } = useTasks();
    const [loading, setLoading] = useState(false);

    const onFinish = async (values) => {
        setLoading(true);
        try {
            await addTask(values);
            form.resetFields();
            message.success('G�rev ba�ar�yla eklendi.');
        } catch  {
            message.error('G�rev eklenirken bir hata olu�tu.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <Form form={form} onFinish={onFinish}>
                <Form.Item name="title" label="Ba�l�k" rules={[{ required: true, message: 'Ba�l�k gereklidir' }]}>
                    <Input />
                </Form.Item>
                <Form.Item>
                    <Button type="primary" htmlType="submit" loading={loading}>Ekle</Button>
                </Form.Item>
            </Form>
        </div>
    );
}

export default GorevEkle;
