import { Backdrop, CircularProgress, Paper, Typography } from "@mui/material";
import axios from "axios";
import { useEffect, useState } from "react";
import { DataGrid, type GridColDef } from "@mui/x-data-grid";
const API_URL = '/api/products';

function ProductsList() {
    const [products, setProducts] = useState([]);
    const [showBackdropLoading, setShowBackdropLoading] = useState(false);

    const columns: GridColDef[] = [
        { field: 'id', headerName: 'ID', width: 40 },
        { field: 'name', headerName: 'Producto', width: 130 },
        { field: 'description', headerName: 'Descripción', width: 130 },
        {
            field: 'price',
            headerName: 'Precio',
            type: 'number',
        }
    ]

    const paginationModel = { page: 0, pageSize: 5 };

    /**
     * Getting the Products Data from API
     */
    useEffect(() => {
        setShowBackdropLoading(true);
        axios.get(API_URL)
            .then((response) => setProducts(response.data))
            .catch(() => {

            }).finally(() => {
                setShowBackdropLoading(false);
            });
    }, []);

    return (
        <>
            <Backdrop
                sx={(theme) => ({ color: '#fff', zIndex: theme.zIndex.drawer + 1 })}
                open={showBackdropLoading} >
                <CircularProgress color="inherit" />
            </Backdrop>
            <Typography variant="h2" gutterBottom fontWeight={"bold"} textAlign={"center"}>Online Store</Typography>
            
            <Typography variant="subtitle1" gutterBottom fontWeight={"bold"} textAlign={"center"}>Consulta nuestros productos</Typography>

            <Paper sx={{ height: 400, width: '600px', margin: 'auto' }}> 
                <DataGrid 
                    rows={products}
                    columns={columns}
                    initialState={{ pagination: { paginationModel } }}
                    pageSizeOptions={[5, 10]}
                    sx={{ border: 0 }}
                />
            </Paper>
        </>
    )
}

export default ProductsList;