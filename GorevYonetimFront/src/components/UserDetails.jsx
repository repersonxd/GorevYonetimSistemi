import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Spin, Card, Typography, Button, message } from 'antd';
import axios from 'axios';

function UserDetails() {
    const { id } = useParams();
    const [user, setUser] = useState(null);  // useState ile state'i ba�lat�n
    const [loading, setLoading] = useState(true);  // useState ile state'i ba�lat�n

    useEffect(() => {
        axios.get(`http://localhost:7257/api/kullanicilar/${id}`)
            .then(response => {
                setUser(response.data);
                setLoading(false);
            })
            .catch(error => {
                console.error('Error fetching user details:', error);
                message.error('Kullan�c� detaylar� al�n�rken bir hata olu�tu.');
                setLoading(false);
            });
    }, [id]);

    if (loading) {
        return <Spin size="large" />;
    }

    return (
        <div>
            <Card title="Kullan�c� Detaylar�">
                <Typography.Paragraph><strong>ID:</strong> {user.id}</Typography.Paragraph>
                <Typography.Paragraph><strong>Kullan�c� Ad�:</strong> {user.kullaniciAdi}</Typography.Paragraph>
                <Typography.Paragraph><strong>Email:</strong> {user.email}</Typography.Paragraph>
                <Button onClick={() => console.log('Edit')} type="primary">D�zenle</Button>
            </Card>
        </div>npm run dev

    );
}

export default UserDetails;
