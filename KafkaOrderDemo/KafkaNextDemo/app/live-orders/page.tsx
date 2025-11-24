"use client";

import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

interface Order {
    customer: string;
    amount: number;
}

export default function LiveOrdersPage() {
    const [orders, setOrders] = useState<Order[]>([]);
    const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
    const [connectionStatus, setConnectionStatus] = useState<string>("در حال اتصال...");

    useEffect(() => {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5054/orderHub")
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        hubConnection.start()
            .then(() => {
                console.log("SignalR Connected");
                setConnectionStatus("متصل");
            })
            .catch(err => {
                console.error("SignalR Connection Error:", err);
                setConnectionStatus("خطا در اتصال");
            });

        hubConnection.onreconnecting(() => {
            setConnectionStatus("در حال اتصال مجدد...");
        });

        hubConnection.onreconnected(() => {
            setConnectionStatus("متصل");
        });

        hubConnection.onclose(() => {
            setConnectionStatus("قطع شده");
        });

        hubConnection.on("NewOrder", (order: Order) => {
            console.log("سفارش جدید دریافت شد:", order);
            setOrders(prev => [order, ...prev]);
        });

        setConnection(hubConnection);

        return () => {
            hubConnection.stop();
        };
    }, []);

    return (
        <div className="min-h-screen bg-gray-100 flex flex-col items-center p-6">
            <h1 className="text-4xl font-bold mb-4">سفارش‌های زنده</h1>

            {/* نمایش وضعیت اتصال */}
            <div className={`mb-6 px-4 py-2 rounded ${
                connectionStatus === "متصل" ? "bg-green-500" :
                    connectionStatus === "خطا در اتصال" ? "bg-red-500" :
                        "bg-yellow-500"
            } text-white`}>
                وضعیت: {connectionStatus}
            </div>

            <div className="mb-6">
                <button
                    onClick={() => setOrders([])}
                    className="px-6 py-2 bg-red-500 text-white rounded hover:bg-red-600 transition"
                >
                    پاک کردن لیست ({orders.length})
                </button>
            </div>

            <div className="w-full max-w-3xl space-y-4">
                {orders.length === 0 && (
                    <div className="bg-white p-8 rounded shadow text-center">
                        <p className="text-gray-500">هنوز سفارشی ثبت نشده.</p>
                        <p className="text-sm text-gray-400 mt-2">
                            در انتظار دریافت سفارش‌های جدید از سرور...
                        </p>
                    </div>
                )}
                {orders.map((order, idx) => (
                    <div
                        key={idx}
                        className="bg-white p-4 rounded shadow flex justify-between items-center hover:shadow-lg transition animate-fade-in"
                    >
                        <div>
                            <span className="font-medium text-lg">{order.customer}</span>
                            <span className="text-xs text-gray-400 block">سفارش #{idx + 1}</span>
                        </div>
                        <span className="text-blue-600 font-semibold text-xl">
                            {order.amount.toLocaleString()} تومان
                        </span>
                    </div>
                ))}
            </div>
        </div>
    );
}