"use client";

import { useState } from "react";
import Link from "next/link";

export default function Home() {
    const [customer, setCustomer] = useState("");
    const [amount, setAmount] = useState(0);
    const [status, setStatus] = useState("");

    const submitOrder = async () => {
        setStatus("در حال ارسال سفارش...");

        try {
            const res = await fetch("http://localhost:5054/api/Orders", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ customer, amount }),
            });
            const data = await res.json();
            
            if (!res.ok) throw new Error(data.message);
            
            setStatus(`سفارش با موفقیت ثبت شد: ${data.orderId}`);
            setCustomer("");
            setAmount(0);
        } catch (err: any) {
            console.error(err);
            setStatus("ثبت سفارش با خطا مواجه شد!");
        }
    };

    return (
        <div className="min-h-screen flex flex-col items-center justify-center bg-gray-100">
            <Link href="/live-orders">
                <button className="mb-4 px-6 py-2 bg-green-500 text-white rounded hover:bg-green-600">
                    مشاهده سفارش‌های زنده
                </button>
            </Link>
            <h1 className="text-3xl font-bold mb-6">ثبت سفارش جدید</h1>

            <input
                type="text"
                placeholder="نام مشتری"
                value={customer}
                onChange={(e) => setCustomer(e.target.value)}
                className="mb-4 p-2 border rounded w-72"
            />

            <input
                type="number"
                placeholder="مبلغ"
                value={amount}
                onChange={(e) => setAmount(parseInt(e.target.value))}
                className="mb-4 p-2 border rounded w-72"
            />

            <button
                onClick={submitOrder}
                className="mb-4 px-6 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
            >
                ثبت سفارش
            </button>

            <p className="text-gray-700">{status}</p>
        </div>
    );
}